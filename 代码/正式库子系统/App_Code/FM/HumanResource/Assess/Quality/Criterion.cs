namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// ÷∏±Í¿‡
	/// </summary>
	public class Criterion{	

		private string name;

		private int type;

		private string explain;

		private int maxScore;

		private int minScore;

		private bool enable;


		/// <summary>
		/// </summary>
		public string Name{	
			get{
				return this.name;
			}	
			set{
				this.name = value;
			}	
		}
	
		/// <summary>
		/// </summary>
		public int Type{	
			get{
				return this.type;
			}	
			set{
				this.type = value;
			}	
		}
	
		/// <summary>
		/// </summary>
		public string Explain{	
			get{
				return this.explain;
			}	
			set{
				this.explain = value;
			}	
		}
	
		/// <summary>
		/// </summary>
		public int MaxScore{	
			get{
				return this.maxScore;
			}	
			set{
				this.maxScore = value;
			}	
		}
	
		/// <summary>
		/// </summary>
		public int MinScore{	
			get{
				return this.minScore;
			}	
			set{
				this.minScore = value;
			}	
		}
	
		/// <summary>
		/// </summary>
		public bool Enable{	
			get{
				return this.enable;
			}	
			set{
				this.enable = value;
			}	
		}
	
	
	}
}