`default_nettype none

module RVTest (
	input clk,
	input resetn,
	output         o_mem_valid,
	output         o_mem_instr,

	output  [31:0] 	o_mem_addr,
	output  [31:0] 	o_mem_wdata,
	output  [ 3:0] 	o_mem_wstrb,
	output 			o_dbg_mem_valid,
	output [31:0] 	o_dbg_mem_rdata,
	output [31:0] 	o_dbg_mem_wdata,
	output         	o_dbg_mem_we
);

	parameter [0:0] BARREL_SHIFTER = 1;
	parameter [0:0] ENABLE_MULDIV = 1;
	parameter [0:0] ENABLE_COMPRESSED = 1;
	parameter [0:0] ENABLE_COUNTERS = 1;
	parameter [0:0] ENABLE_IRQ_QREGS = 0;

	parameter integer MEM_WORDS = 256;
	parameter [31:0] STACKADDR = 32'h 0000_0800;// (2*MEM_WORDS);       // end of memory
	parameter [31:0] PROGADDR_RESET = 32'h 0000_0000;
	parameter [31:0] PROGADDR_IRQ = 32'h 0020_0000;
	
	// picorv signals
	wire 		cpu_mem_valid;
	wire 		cpu_mem_instr;
	wire 		cpu_mem_ready;
	wire [31:0] cpu_mem_addr;
	wire [31:0] cpu_mem_wdata;
	wire [3:0] 	cpu_mem_wstrb;
	wire [31:0] cpu_mem_rdata;
	
	// application code and data
	reg [31:0] 	l_mem [0:511];	
	reg 		l_mem_ready = 1'b0;
	reg [31:0] 	l_mem_rdata = 32'b0;
	wire [31:0] l_mem_wdata;	
	wire 		l_mem_we;
	reg [1:0] 	l_mem_write_state = 2'b0;
	wire 		l_mem_address_valid;
	
	// byteenabled write
	assign l_mem_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : l_mem_rdata[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : l_mem_rdata[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : l_mem_rdata[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : l_mem_rdata[7:0]
	};
	
	assign l_mem_address_valid = cpu_mem_addr[31:2] < STACKADDR;
	assign l_mem_we = l_mem_write_state == 2'b1 && l_mem_address_valid;
	
	// memory logic
	always @(posedge clk)
	begin
		if (l_mem_we)
		begin
			l_mem[cpu_mem_addr[31:2]] <= l_mem_wdata;
		end
		
		l_mem_rdata <= l_mem[cpu_mem_addr[31:2]];
	end
	
	// tx handling logic
	always @(posedge clk)
	begin
		l_mem_ready <= 1'b0;
		l_mem_write_state <= 2'b0;
		
		if (resetn && cpu_mem_valid && l_mem_address_valid)
		begin		
			l_mem_ready <= 	cpu_mem_instr || 
							cpu_mem_wstrb == 4'b0 || 
							(cpu_mem_wstrb != 4'b0 && l_mem_write_state == 2'b1);
				
			if (!cpu_mem_instr)
			begin
				case (l_mem_write_state)
					0: begin
						l_mem_write_state <= 2'b1;
						// read ready in next cycle
					end
					1: begin
						l_mem_write_state <= 2'b0;
					end
				endcase
			end
		end
	end
	
// BEGIN DATA_DECL
// END DATA_DECL

// BEGIN DATA_CTRL
// END DATA_CTRL

	// additional buffer
	reg [31:0] 	l_buff [0:16];	
	reg 		l_buff_ready = 1'b0;
	reg [31:0] 	l_buff_rdata = 32'b0;
	wire [31:0] l_buff_wdata;	
	wire 		l_buff_we;
	wire 		l_buff_address_valid;

	assign l_buff_address_valid = cpu_mem_addr[31:20] == 12'h002;
	assign l_buff_we = cpu_mem_wstrb != 4'b0 && l_buff_address_valid;

	// memory logic
	always @(posedge clk)
	begin
		if (l_buff_we)
		begin
			l_buff[cpu_mem_addr[31:2]] <= l_mem_wdata;
		end
		
		l_buff_rdata <= l_buff[cpu_mem_addr[31:2]];
	end
	
	// tx handling logic
	always @(posedge clk)
	begin
		l_buff_ready <= resetn && cpu_mem_valid && l_buff_address_valid;
	end
								

	// feedback to cpu
	assign cpu_mem_ready = 		l_mem_ready || l_buff_ready;
	assign cpu_mem_rdata = 
	l_mem_ready ?
		l_mem_rdata
		: l_buff_ready 
			? l_buff_rdata
			: 32'b0;
		
	assign o_mem_valid = 	cpu_mem_valid;
	assign o_mem_instr = 	cpu_mem_instr;
	assign o_mem_addr = 	cpu_mem_addr;
	assign o_mem_wdata = 	cpu_mem_wdata;
	assign o_mem_wstrb = 	cpu_mem_wstrb;
	
	picorv32 #(
		.STACKADDR(STACKADDR),
		.PROGADDR_RESET(PROGADDR_RESET),
		.PROGADDR_IRQ(PROGADDR_IRQ),
		.BARREL_SHIFTER(BARREL_SHIFTER),
		.COMPRESSED_ISA(ENABLE_COMPRESSED),
		.ENABLE_COUNTERS(ENABLE_COUNTERS),
		.ENABLE_MUL(ENABLE_MULDIV),
		.ENABLE_DIV(ENABLE_MULDIV),
		.ENABLE_IRQ(1),
		.ENABLE_IRQ_QREGS(ENABLE_IRQ_QREGS)
	) cpu (
		.clk         (clk        	),
		.resetn      (resetn     	),
		.mem_valid   (cpu_mem_valid ),
		.mem_instr   (cpu_mem_instr ),
		.mem_ready   (cpu_mem_ready ),
		.mem_addr    (cpu_mem_addr  ),
		.mem_wdata   (cpu_mem_wdata ),
		.mem_wstrb   (cpu_mem_wstrb ),
		.mem_rdata   (cpu_mem_rdata ),
		.irq         (0        	 	)
	);
	
	// connect debug and diagnostics
			
	assign o_dbg_mem_valid = l_mem_ready;
	assign o_dbg_mem_rdata = l_mem_rdata;
	assign o_dbg_mem_wdata = l_mem_wdata;
	assign o_dbg_mem_we = l_mem_we;
	
	
	initial
	begin
// BEGIN MEM_INIT
// END MEM_INIT
end

	
endmodule