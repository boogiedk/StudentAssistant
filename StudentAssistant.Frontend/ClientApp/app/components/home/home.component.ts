import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent {
    _http: Http;

    public constructor(http: Http) {
        this._http = http;
    }

    onSubmit(userFeedbackRequestModel: UserFeedbackRequestModel) {
        this._http.post('http://localhost:18936/api/support/sendfeedback', userFeedbackRequestModel).subscribe(status => console.log(JSON.stringify(status)));
    }
}

export interface UserFeedbackRequestModel {

    userName: string;
    emailTo: string;
    subject: string;
    textBody: string;
}



