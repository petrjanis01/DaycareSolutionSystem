import { Component, OnInit } from '@angular/core';
import { TestService } from 'src/app/api/generated';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(private testServ: TestService) { }

  ngOnInit(): void {
  }

  test() {
    this.testServ.apiTestTestPost('asdas', 'asdasda').then(x => console.log(x));
  }
}
