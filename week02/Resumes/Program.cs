using System;

class Program
{
    static void Main(string[] args)
    {
        // Create first job
        Job job1 = new Job();
        job1._jobTitle = "Line Cook";
        job1._company = "McDonalds";
        job1._startYear = 2019;
        job1._endYear = 2022;

        // Create second job
        Job job2 = new Job();
        job2._jobTitle = "Crew Member";
        job2._company = "McDonalds LLC";
        job2._startYear = 2022;
        job2._endYear = 2023;

        // Create resume
        Resume myResume = new Resume();
        myResume._personName = "Amanda Jones";

        // Add jobs to resume
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        // Display resume
        myResume.Display();
    }
}
