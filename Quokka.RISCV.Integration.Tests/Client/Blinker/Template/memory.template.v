	reg 		{NAME}_ready = 1'b0;
	reg [31:0] 	{NAME}_rdata = 32'b0;
	wire [31:0] {NAME}_wdata;	
	wire 		{NAME}_we;
	reg [1:0] 	{NAME}_write_state = 2'b0;
	wire 		{NAME}_address_valid;

		// byteenabled write
	assign {NAME}_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : {NAME}_rdata[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : {NAME}_rdata[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : {NAME}_rdata[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : {NAME}_rdata[7:0]
	};
	
	assign {NAME}_address_valid = cpu_mem_addr[31:24] == 8'h{SEG};
	assign {NAME}_we = {NAME}_write_state == 2'b1 && {NAME}_address_valid;
	
	// memory logic
	always @(posedge clk)
	begin
		if ({NAME}_we)
		begin
			{NAME}[cpu_mem_addr[31:2]] <= {NAME}_wdata;
		end
		
		{NAME}_rdata <= {NAME}[cpu_mem_addr[31:2]];
	end
	
	// tx handling logic
	always @(posedge clk)
	begin
		{NAME}_ready <= 1'b0;
		{NAME}_write_state <= 2'b0;
		
		if (resetn && cpu_mem_valid && {NAME}_address_valid)
		begin		
			{NAME}_ready <= 	cpu_mem_instr || 
							cpu_mem_wstrb == 4'b0 || 
							(cpu_mem_wstrb != 4'b0 && {NAME}_write_state == 2'b1);
				
			if (!cpu_mem_instr)
			begin
				case ({NAME}_write_state)
					0: begin
						{NAME}_write_state <= 2'b1;
						// read ready in next cycle
					end
					1: begin
						{NAME}_write_state <= 2'b0;
					end
				endcase
			end
		end
	end