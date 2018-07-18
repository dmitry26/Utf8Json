using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Utf8Json.Tests
{
	public class PropertyWithRefReturn
	{
		public struct MyId
		{
			public string Label;
			public int Int1;
			public double Dbl1;
		}

		public class ClassWithRef
		{
			private MyId _id;

			[SerializationConstructor]
			public ClassWithRef(MyId id)
			{
				_id = id;
			}

			public ref MyId Id
			{
				get => ref _id;
			}

			public string Name;
		}

		[Fact]
		public void ShoudSerializeClassWithRef()
		{
			var id = new MyId
			{
				Label = "Test1",
				Int1 = 1,
				Dbl1 = 1.34
			};

			var o = new ClassWithRef(id)
			{
				Name = "Name1"
			};

			var s1 = JsonSerializer.ToJsonString(o);

			s1.Is("{\"Id\":{\"Label\":\"Test1\",\"Int1\":1,\"Dbl1\":1.34},\"Name\":\"Name1\"}");

			var bin = JsonSerializer.Serialize(o);
			var o2 = JsonSerializer.Deserialize<ClassWithRef>(bin);

			//Assert can't handle ref return properties
			o2.Id.Is(o.Id);
			o2.Name.Is(o.Name);
		}		
	}
}
