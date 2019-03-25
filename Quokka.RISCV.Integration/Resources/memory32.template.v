	// 32 bit memory logic for {NAME}
	wire 			{NAME}_ready;
	reg 			{NAME}_read_ready = 1'b0;
	reg 			{NAME}_write_ready = 1'b0;
	reg  [31:0] {NAME}_rdata = 32'b0;
	wire [31:0] {NAME}_wdata;	
	wire [31: 0]	{NAME}_address;
	wire 			{NAME}_we;
	reg  [1:0] 		{NAME}_write_state = 2'b0;
	wire 			{NAME}_address_valid;

	assign {NAME}_address_valid = cpu_mem_addr[31:{SEG_END}] == {SEG_WIDTH}'h{SEG};
	assign {NAME}_ready = {NAME}_read_ready || {NAME}_write_ready;
	
	assign {NAME}_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : {NAME}_rdata[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : {NAME}_rdata[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : {NAME}_rdata[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : {NAME}_rdata[7:0]
	};

	assign {NAME}_we = {NAME}_write_state == 2'b1 && {NAME}_address_valid;
	assign {NAME}_address = cpu_mem_addr[31:2];
	
	always @(posedge clk)
	begin
		{NAME}_read_ready <= 1'b0;
		{NAME}_write_ready <= 1'b0;
		{NAME}_write_state <= 2'b0;
		{NAME}_rdata <= 32'b0;

		if (resetn && cpu_mem_valid && {NAME}_address_valid)
			begin	
				{NAME}_read_ready <= cpu_read_request;
				{NAME}_write_ready <= cpu_write_request && {NAME}_write_state == 2'b1;
			
				if (cpu_write_request)
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

		if ({NAME}_we)
			begin
				{NAME}[{NAME}_address] <= {NAME}_wdata;
			end

		{NAME}_rdata <= {NAME}[{NAME}_address];
	end
