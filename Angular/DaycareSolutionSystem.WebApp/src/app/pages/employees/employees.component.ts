import { Component, OnInit } from '@angular/core';
import { EmployeeBasicDTO, EmployeeService } from 'src/app/api/generated';
import { Router } from '@angular/router';
import { GeneralHelperService } from 'src/app/services/general-helper.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})
export class EmployeesComponent implements OnInit {
  public employeeBasics: EmployeeBasicDTO[];
  public employeeNameText: string;

  constructor(private employeeService: EmployeeService, private router: Router,
    private helper: GeneralHelperService) { }

  async ngOnInit() {
    let dtos = await this.employeeService.apiEmployeeGet();

    dtos.forEach(dto => {
      if (dto.profilePictureUri == null) {
        dto.profilePictureUri = this.helper.getAnonymousImgUrlFormatted();
      }
    });

    this.employeeBasics = dtos;
  }

  public openEmployeeDetail(id: string) {
    this.router.navigate(['employees/detail', id]);
  }

  public addNewEmployee() {
    this.router.navigate(['employees/detail', 0])
  }
}
