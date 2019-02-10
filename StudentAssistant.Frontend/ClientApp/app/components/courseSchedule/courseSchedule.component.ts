import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Component } from '@angular/core';
import { getCourseScheduleTodayApi } from '../../app.browser.module';
import { getCourseScheduleTomorrowApi } from '../../app.browser.module';

@Component({
    selector: 'courseSchedule',
    templateUrl: './courseSchedule.component.html',
    styleUrls: ['./courseSchedule.component.css']
})
export class CourseScheduleComponent {
    public courses: CourseScheduleModel[];
    private _baseUrl: string;


    constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
        http.get(baseUrl + getCourseScheduleTodayApi()).subscribe(result => { this.courses = result.json() as CourseScheduleModel[] }, error => console.error(error));
    }

    getCourseScheduleTomorrow() {
        this.http.get(this._baseUrl + getCourseScheduleTomorrowApi()).subscribe(result => { this.courses = result.json() as CourseScheduleModel[] }, error => console.error(error));
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
