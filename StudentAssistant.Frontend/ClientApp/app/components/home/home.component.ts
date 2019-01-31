import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent {

    public constructor(private http: Http) {

    }

    onSubmit(userFeedbackRequestModel: UserFeedbackRequestModel) {
        this.http.post('http://localhost:18936/api/support/sendfeedback', userFeedbackRequestModel).subscribe(status => console.log(JSON.stringify(status)));
    }
}

export interface UserFeedbackRequestModel {

    userName: string;
    emailTo: string;
    subject: string;
    textBody: string;
}



