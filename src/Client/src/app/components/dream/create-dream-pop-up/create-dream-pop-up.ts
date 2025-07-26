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

  constructor(private dreamsService: Dreams) {
    const fb = inject(FormBuilder);
    this.form = fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      imageBase64: ['']
    });
  }

  submit() {
    if (this.form.valid) {
      const dream: Partial<Dream> = {
        title: this.form.value.title,
        description: this.form.value.description,
        imageBase64: this.form.value.imageBase64,
        producerId: '',
        rating: 0,
        categories: [],
        imageContentType: this.form.value.imageContentType
      };

      this.dreamsService.addDream(dream as Dream).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];
    const reader = new FileReader();

    reader.onload = () => {
      const base64 = (reader.result as string).split(',')[1]; // Чистый base64 без `data:image/...`
      const contentType = file.type;

      this.form.patchValue({
        imageBase64: base64
      });
      this.form.patchValue({ imageContentType: contentType });
    };

    reader.readAsDataURL(file);
  }

}
