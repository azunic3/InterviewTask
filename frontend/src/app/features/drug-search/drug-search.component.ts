import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DrugsService, DrugSearchResponse } from '../../core/services/drugs.service';

@Component({
  selector: 'app-drug-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './drug-search.component.html',
  styleUrls: ['./drug-search.component.scss']
})
export class DrugSearchComponent {
  query = '';
  result: DrugSearchResponse | null = null;
  error: string | null = null;

  constructor(private drugs: DrugsService) {}

  onSearch() {
    this.error = null;
    this.result = null;

    const q = this.query.trim();
    if (!q) {
      this.error = 'Unesi naziv lijeka.';
      return;
    }

    this.drugs.search(q).subscribe({
      next: res => this.result = res,
      error: err => this.error = err?.error ?? 'Greška pri pozivu backend API-ja.'
    });
  }
}
