using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common {
	public class RigidBodyAnimation {
		
	}

	public class RigidBody {
		public RigidBody(Point position, Size size) {
			this.position = position;
			this.size = size;
		}
		Point position = new Point();
		Size size = new Size();

	}

	public partial class AnimationForm : Form {
		public AnimationForm() {
			InitializeComponent();
		}
	}
}
