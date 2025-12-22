import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-students',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './students.component.html',
  styleUrl: './students.component.scss'
})
export class StudentsComponent implements OnInit {

  students: any[] = [];
  model: any = {};
  isEdit = false;
  editId: number | null = null;

  constructor(private studentService: StudentService) {}

  ngOnInit() {
    this.loadStudents();
  }

  loadStudents() {
    this.studentService.getStudents().subscribe(res => {
      this.students = res;
    });
  }

  save() {
    if (this.isEdit && this.editId !== null) {
      this.studentService.updateStudent(this.editId, this.model)
        .subscribe(() => {
          this.reset();
          this.loadStudents();
        });
    } else {
      this.studentService.addStudent(this.model)
        .subscribe(() => {
          this.reset();
          this.loadStudents();
        });
    }
  }

  edit(student: any) {
    this.model = { ...student };
    this.editId = student.id;
    this.isEdit = true;
  }

  delete(id: number) {
    this.studentService.deleteStudent(id)
      .subscribe(() => this.loadStudents());
  }

  reset() {
    this.model = {};
    this.isEdit = false;
    this.editId = null;
  }
}
