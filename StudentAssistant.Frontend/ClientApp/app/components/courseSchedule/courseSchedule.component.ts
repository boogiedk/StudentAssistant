import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Component } from '@angular/core';
import { getCourseScheduleApi } from '../../app.browser.module';

@Component({
    selector: 'courseSchedule',
    templateUrl: './courseSchedule.component.html',
    styleUrls: ['./courseSchedule.component.css']
})
export class CourseScheduleComponent {
    public courses: CourseScheduleModel[];


    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + getCourseScheduleApi()).subscribe(result => { this.courses = result.json() as CourseScheduleModel[] }, error => console.error(error));
    }
}

interface CourseScheduleModel {
    nameOfDayWeek: string;
    coursesViewModel: CoursesViewModel[];
}

interface CoursesViewModel {
    courseName: string;
    courseNumber: number;
    courseType: string;
    teacherFullName: string;
    numberWeek: string;
    parityWeek: string;
    coursePlace: string;
}
