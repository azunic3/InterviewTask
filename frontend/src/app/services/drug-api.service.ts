import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PopularSearch {
  query: string;
  count: number;
}

@Injectable({
  providedIn: 'root'
})
export class DrugApiService {

  // backend base URL
  private readonly baseUrl = 'https://localhost:7091/api';

  constructor(private http: HttpClient) {}

  getPopularSearches(limit: number = 5): Observable<PopularSearch[]> {
    return this.http.get<PopularSearch[]>(
      `${this.baseUrl}/drugs/popular?limit=${limit}`
    );
  }

  searchDrug(query: string, allergens?: string) {
    let url = `${this.baseUrl}/drugs/search?query=${encodeURIComponent(query)}`;
    if (allergens) {
      url += `&allergens=${encodeURIComponent(allergens)}`;
    }
    return this.http.get(url);
  }

  notifyAvailability(drugKey: string, email: string) {
  return this.http.post<any>(`${this.baseUrl}/availability-requests/notify`, {
    drugKey,
    email
  });
}

}
