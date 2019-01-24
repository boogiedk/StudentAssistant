import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Component } from '@angular/core';

@Component({
    selector: 'parityOfTheWeek',
    templateUrl: './parityOfTheWeek.component.html',
    styleUrls: ['./parityOfTheWeek.component.css']
})
export class ParityOfTheWeekComponent {
    public parityOfTheWeekModel: ParityOfTheWeekModel;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get('http://localhost:18936/api/parity/today').subscribe(result => {
            this.parityOfTheWeekModel = result.json() as ParityOfTheWeekModel; //parityOfTheWeekModel
        }, error => console.error(error));
    }
}

interface ParityOfTheWeekModel {
    dateTimeRequest: string;
    parityOfWeekToday: string;
    parityOfWeekCount: number;
    partOfSemester: number;
    dayOfName: string;
    numberOfSemester: number;
    statusDay: string;
    isParity: boolean;
}
