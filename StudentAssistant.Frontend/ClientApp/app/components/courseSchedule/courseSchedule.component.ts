import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Component } from '@angular/core';

@Component({
    selector: 'courseSchedule',
    templateUrl: './courseSchedule.component.html',
    styleUrls: ['./courseSchedule.component.css']
})
export class CourseScheduleComponent {
    //public courseScheduleModel: CourseScheduleModel = new CourseScheduleModel();
    //  public courseScheduleModels: CourseScheduleModel[];

    //    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
    //        http.get(baseUrl + getCourseScheduleApi()).subscribe((data: CourseScheduleModel[]) => this.courseScheduleModels = data);
    //    }
    //}
}
// class CourseScheduleModel {
//    constructor(
//       public parityWeek?: boolean,
//        public nameOfDayWeek?: string,
//        public courseName?: string,
//        public courseNumber?: number,
//        public courseType?: number,
//        public teacherFullName?: string) {}
//}
