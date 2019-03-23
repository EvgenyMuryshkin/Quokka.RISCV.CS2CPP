	// 16 bit memory logic for {NAME}
	wire 			{NAME}_ready;
	reg 			{NAME}_read_ready = 0;
	reg 			{NAME}_write_ready = 0;
	reg  [31:0]		{NAME}_rdata = 0;
	reg  [15:0]		{NAME}_rdata_part = 0;
	wire [15:0]		{NAME}_wdata;	
	wire [31: 0]	{NAME}_read_address;
	wire [31: 0]	{NAME}_write_address;
	wire 			{NAME}_we;
	reg  [2:0] 		{NAME}_read_state = 0;
	wire 			{NAME}_address_valid;
	reg				{NAME}_read_address_part = 0;
	wire			{NAME}_write_address_part;

	assign o_dbg_tmp = {NAME}_rdata;

	assign {NAME}_address_valid = cpu_mem_addr[31:{SEG_END}] == {SEG_WIDTH}'h{SEG};
	assign {NAME}_ready = {NAME}_read_ready || {NAME}_write_ready;
	
	// could it be wrong? sure it can, but will deal later
	assign {NAME}_wdata = cpu_mem_wdata[15:0];

	assign {NAME}_we = cpu_write_request && cpu_mem_valid && {NAME}_address_valid;
	assign {NAME}_read_address = { cpu_mem_addr[31:2], {NAME}_read_address_part };
	assign {NAME}_write_address = { cpu_mem_addr[31:2], {NAME}_write_address_part };
	
	assign {NAME}_write_address_part = {
		cpu_mem_wstrb[3] ? 1 : 
		0
	};

	always @(posedge clk)
	begin
		{NAME}_read_ready <= 0;
		{NAME}_write_ready <= 0;
		{NAME}_read_address_part <= 0;

		if (resetn && cpu_mem_valid && {NAME}_address_valid)
			begin	
				{NAME}_read_ready <= cpu_read_request && {NAME}_read_state == 3'b011;
				{NAME}_write_ready <= cpu_write_request;
			
				// 2 cycle read
				if (cpu_read_request && {NAME}_read_state <= 3'b011)
					begin
						{NAME}_read_state <= {NAME}_read_state + 1;
						if ({NAME}_read_state != 0)
							begin
								{NAME}_read_address_part <= {NAME}_read_address_part + 1;
								{NAME}_rdata <= { {NAME}_rdata_part, {NAME}_rdata[31:16] };
							end
					end
			end
		else
			begin
				{NAME}_read_state <= 0;
			end
	end

always @(posedge clk)
	begin
		{NAME}_rdata_part <= 0;

		if (resetn)
			begin
				if ({NAME}_we)
					begin
						{NAME}[{NAME}_write_address] <= {NAME}_wdata;
					end

				{NAME}_rdata_part <= {NAME}[{NAME}_read_address];
			end
	end