import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.shared.module';
import { AppComponent } from './components/app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return "http://localhost:18936";
}

export function getParityOfTheWeekApi() {
    return "/api/parity/today";
}

export function getUserSupportApi() {
    return "/api/support/sendfeedback";
}

export function getCourseScheduleTodayApi() {
    return "/api/schedule/today";
}

export function getCourseScheduleTomorrowApi() {
    return "/api/schedule/tomorrow";
}

export function getSwaggerApi() {
    return "/swagger";
}
