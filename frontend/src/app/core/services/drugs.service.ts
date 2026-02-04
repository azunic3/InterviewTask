import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environments';

export interface DrugSearchResponse {
  query: string;
  results: Array<{ name: string; warning: string; source: string }>;
}

@Injectable({ providedIn: 'root' })
export class DrugsService {
  private baseUrl = `${environment.apiUrl}/api/drugs`;

  constructor(private http: HttpClient) {}

  search(query: string) {
    return this.http.get<DrugSearchResponse>(`${this.baseUrl}/search`, {
      params: { query }
    });
  }
}
