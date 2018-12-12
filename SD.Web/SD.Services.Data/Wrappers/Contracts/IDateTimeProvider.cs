using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Services.Data.Wrappers.Contracts
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}
