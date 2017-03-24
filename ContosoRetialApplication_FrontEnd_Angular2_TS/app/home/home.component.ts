import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    templateUrl: 'app/home/home.component.html'
})
export class HomeComponent {
    title: string = 'SMATA Home';

    constructor(private _router: Router) { }

    onDevExtremeClick(): void {
        this._router.navigate(['/home/contosoretail']);
    }
}