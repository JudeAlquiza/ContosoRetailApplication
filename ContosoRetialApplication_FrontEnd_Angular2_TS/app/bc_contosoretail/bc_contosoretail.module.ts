
import { NgModule } from '@angular/core';
import { CommonModule }  from '@angular/common';
import { RouterModule }   from '@angular/router';
import { HttpModule }   from '@angular/http';

import { DxDataGridModule } from 'devextreme-angular'; 
import { DxButtonModule } from 'devextreme-angular';

import { BC_ContosoRetailComponent } from './bc_contosoretail.component'

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild([
            { path: 'home/contosoretail', component: BC_ContosoRetailComponent },
        ]),
        HttpModule,
        DxDataGridModule,
        DxButtonModule
    ],
    declarations: [
        BC_ContosoRetailComponent
    ],
    providers: []
})
export class ContosoRetailModule { }