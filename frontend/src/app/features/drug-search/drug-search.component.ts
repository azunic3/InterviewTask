import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-drug-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './drug-search.component.html',
  styleUrls: ['./drug-search.component.scss']
})
export class DrugSearchComponent {
  
}
