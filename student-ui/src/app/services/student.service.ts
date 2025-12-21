import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private apiUrl = 'https://localhost:44332/api/student';

  constructor(private http: HttpClient) {}

  getStudents() {
    return this.http.get<any[]>(`${this.apiUrl}/get`);
  }

  addStudent(data: any) {
    return this.http.post(`${this.apiUrl}/add`, data);
  }

  updateStudent(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/update/${id}`, data);
  }

  deleteStudent(id: number) {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
