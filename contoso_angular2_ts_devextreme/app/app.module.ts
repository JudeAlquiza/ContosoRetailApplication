import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

import { DxButtonModule } from 'devextreme-angular';

import { ContosoRetailModule } from './bc_contosoretail/bc_contosoretail.module';

@NgModule({
  imports: [
    BrowserModule,
    HttpModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: '', redirectTo: 'home', pathMatch: 'full' }
    ]),
    DxButtonModule,
    ContosoRetailModule
  ],
  declarations: [
    AppComponent,
    HomeComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
