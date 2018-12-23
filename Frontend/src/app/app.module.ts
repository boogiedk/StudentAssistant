import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatMenuModule} from '@angular/material/menu';

import { AppRoutingModule } from './app-routing.module';
import { ParityComponent } from './components/parityOfTheWeek/parityOfTheWeek.component';
import {MenuOverview} from './components/navmenu/navmenu.component'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    ParityComponent,
    MenuOverview
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    MatMenuModule,
    BrowserAnimationsModule
  ],
  entryComponents: [MenuOverview],
  providers: [],
  bootstrap: [
    ParityComponent,
    MenuOverview
  ]
})
export class AppModule { }
