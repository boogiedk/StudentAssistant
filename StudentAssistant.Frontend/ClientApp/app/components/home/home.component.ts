import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { getUserSupportApi } from '../../app.browser.module';
import { Inject } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent {
    private _baseUrl: string;

    public constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
    }

    onSubmit(userFeedbackRequestModel: UserFeedbackRequestModel) {
        this.http.post(this._baseUrl + getUserSupportApi(), userFeedbackRequestModel)
            .subscribe(status => console.log(JSON.stringify(status)));
    }
}

export interface UserFeedbackRequestModel {

    userName: string;
    emailTo: string;
    subject: string;
    textBody: string;
}



