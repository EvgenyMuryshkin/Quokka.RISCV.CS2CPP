	wire 		{NAME}_ready;
	wire 		{NAME}_we;
	wire [31:0] {NAME}_wdata;	

	// byteenabled write
	assign {NAME}_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : {NAME}[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : {NAME}[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : {NAME}[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : {NAME}[7:0]
	};
	
	assign {NAME}_ready = cpu_mem_addr[31:24] == 8'h{SEG};
	assign {NAME}_we = {NAME}_ready && !cpu_mem_instr;
	
	// memory logic
	always @(posedge clk)
	begin
		if (resetn)
			begin
				if ({NAME}_we)
					begin
						{NAME} <= {NAME}_wdata;
					end
			end
		else
			begin
				{NAME} <= 32'b0;
			end
	end
	
