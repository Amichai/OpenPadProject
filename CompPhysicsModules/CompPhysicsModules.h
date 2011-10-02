#pragma once


namespace CompPhysicsModules{
	class ProjectileLaunch{
	public:
		//ProjectileLaunch();
		ProjectileLaunch(double);
		ProjectileLaunch(double, double);
		ProjectileLaunch(double, double, double);
	};
	class ProjectileMotion{
	public:
		ProjectileMotion();
		void GetAngleToTarget(int);
		
	};
	class Angle{
	public:
		Angle();
	};
	class Target{
	public:
		double x,y;
		Target(double, double);
	};
}