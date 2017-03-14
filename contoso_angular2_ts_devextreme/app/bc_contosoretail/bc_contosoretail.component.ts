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
                var params = '?';

                if (loadOptions.skip) {
                    params += 'skip=' + JSON.stringify(loadOptions.skip);
                }
                else {
                    params += 'skip=0';
                }

                if (loadOptions.take) {
                    params += '&take=' + JSON.stringify(loadOptions.take);
                }
                else {
                    params += '&take=10';
                }

                if (loadOptions.sort) {
                    params += '&sort=' + JSON.stringify(loadOptions.sort);
                }

                if (loadOptions.filter) {
                    params += '&filter=' + JSON.stringify(loadOptions.filter);
                }

                if (loadOptions.group) {
                    params += '&group=' + JSON.stringify(loadOptions.group);
                }

                if (loadOptions.requireTotalCount) {
                    params += '&requireTotalCount=' + JSON.stringify(loadOptions.requireTotalCount);
                }

                if (loadOptions.requireGroupCount) {
                    params += '&requireGroupCount=' + JSON.stringify(loadOptions.requireGroupCount);
                }

                if (loadOptions.remoteGrouping) {
                    params += '&remoteGrouping=' + JSON.stringify(loadOptions.remoteGrouping);
                }
                console.log(params);
                return http.get('http://localhost:54555/api/customerorders' + params)
                    .toPromise()
                    .then(response => {
                        var json = response.json();
                        console.log(JSON.stringify(json));
                        return {
                            data: json.items,
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