import { Http } from '@angular/http';
import { Component } from '@angular/core';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public parityOfTheWeekModel: ParityOfTheWeekModel;

  constructor(http: Http) {
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
}

