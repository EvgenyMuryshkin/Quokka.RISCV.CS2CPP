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
	output         	o_dbg_mem_we,
	output         	o_dbg_data_access,
	output         	o_dbg_mem_read,
	output         	o_dbg_mem_write,
	output [31:0]   o_dbg_tmp
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
	
	wire		cpu_read_request;
	wire		cpu_write_request;
	assign cpu_read_request = cpu_mem_instr || cpu_mem_wstrb == 4'b0;
	assign cpu_write_request = cpu_mem_wstrb != 4'b0;

// BEGIN DATA_DECL
// END DATA_DECL

// BEGIN DATA_CTRL
// END DATA_CTRL

	// feedback to cpu
	assign cpu_mem_ready = {MEM_READY};
	assign cpu_mem_rdata = {MEM_RDATA};
		
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
			
	assign o_dbg_mem_valid = firmware_ready;
	assign o_dbg_mem_rdata = firmware_rdata;
	assign o_dbg_mem_wdata = firmware_wdata;
	assign o_dbg_mem_we = firmware_we;

	//assign o_dbg_mem_rdata = data_rdata_part;
	//assign o_dbg_mem_wdata = data_read_address_part;
	//assign o_dbg_mem_we = data_we;
	
	//assign o_dbg_data_access = data_address_valid;
	assign o_dbg_mem_read = cpu_read_request;
	assign o_dbg_mem_write = cpu_write_request;
	
	initial
	begin
// BEGIN MEM_INIT
// END MEM_INIT
end

	
endmodule