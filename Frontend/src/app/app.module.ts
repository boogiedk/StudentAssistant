import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppRoutingModule } from './app-routing.module';
import { ParityComponent } from './components/parityOfTheWeek/parityOfTheWeek.component';

@NgModule({
  declarations: [
    ParityComponent,
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [ParityComponent]
})
export class AppModule { }
