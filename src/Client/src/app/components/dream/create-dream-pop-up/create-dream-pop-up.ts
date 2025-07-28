import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Dream, Dreams} from '../../../services/dreams/dreams';
import {MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-create-dreams-pop-up',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './create-dream-pop-up.html',
  styleUrl: './create-dream-pop-up.scss'
})
export class CreateDreamPopUp {
  form: FormGroup;
  dialogRef = inject(MatDialogRef<CreateDreamPopUp>);
  private selectedFile: File | null = null;

  constructor(private dreamsService: Dreams) {
    const fb = inject(FormBuilder);
    this.form = fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      this.selectedFile = null;
      return;
    }
    this.selectedFile = input.files[0];
  }

  submit() {
    if (this.form.invalid) return;

    const formData = new FormData();
    formData.append('Title', this.form.value.title);
    formData.append('Description', this.form.value.description);
    formData.append('ProducerId', '');
    formData.append('Rating', '0');

    if (this.selectedFile) {
      formData.append('Image', this.selectedFile, this.selectedFile.name);
    }

    this.dreamsService.addDream(formData).subscribe(() => {
      this.dialogRef.close(true);
    });
  }
}
