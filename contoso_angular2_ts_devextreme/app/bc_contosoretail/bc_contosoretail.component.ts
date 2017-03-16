import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import CustomStore from 'devextreme/data/custom_store';
import { createStore } from 'devextreme-aspnet-data';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Component({
    templateUrl: 'app/bc_contosoretail/bc_contosoretail.component.html'
})
export class BC_ContosoRetailComponent {

    customerOrders: any = {};
    errorMessage: any;

    constructor(private _router: Router, http: Http) {
        this.customerOrders = new CustomStore({
            load: function (loadOptions: any) {
                
                console.log(loadOptions);
                
                var params: string = '';
                
                // skip
                if (loadOptions.skip){
                    params += 'skip=' + loadOptions.skip;
                }
                else{
                    params += 'skip=0';
                }

                // take
                if (loadOptions.take){
                    params += '&take=' + loadOptions.take;
                }
                else{
                    params += '&take=0';
                }

                // filter
                if (loadOptions.filter){
                    params += '&filter=' + JSON.stringify(loadOptions.filter);
                }

                // sort
                if (loadOptions.sort){
                    params += '&sort=' + JSON.stringify(loadOptions.sort);
                }

                // group
                if (loadOptions.group){
                    params += '&group=' + JSON.stringify(loadOptions.group);
                }

                // requireTotalCount
                if (loadOptions.requireTotalCount){
                    params += '&requireTotalCount=' + loadOptions.requireTotalCount;
                }
                else{
                    params += '&requireTotalCount=false';
                }

                // requireGroupCount
                if (loadOptions.requireGroupCount){
                    params += '&requireGroupCount=' + loadOptions.requireGroupCount;
                }
                else{
                    params += '&requireGroupCount=false';
                }

                // searchExpr
                if (loadOptions.searchExpr){
                    params += '&searchExpr=' + loadOptions.searchExpr;
                }

                // searchOperation
                if (loadOptions.searchOperation){
                    params += '&searchOperation=' + loadOptions.searchOperation;
                }

                // searchValue
                if (loadOptions.searchValue){
                    params += '&searchValue=' + loadOptions.searchValue;
                }

                // totalSummary
                if (loadOptions.totalSummary){
                    params += '&totalSummary=' + JSON.stringify(loadOptions.totalSummary);
                }

                console.log(params);
                
                return http.get('http://localhost:54555/api/customerorders?' + params)
                    .toPromise()
                    .then(response => {
                        var json = response.json();
                        console.log(json);
                        return {
                            data: json.data,
                            totalCount: json.totalCount,
                            groupCount: json.groupCount,
                            summary: [ json.totalCount, json.totalAmount ]
                            };}
                     )
                    .catch(error => { throw 'Data Loading Error' });
            }
        });
    }

    onBack() {
        this._router.navigate(['/home']);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }

}