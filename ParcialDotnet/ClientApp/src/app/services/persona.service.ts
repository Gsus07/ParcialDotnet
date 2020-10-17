import { Injectable } from '@angular/core';
import { Persona } from '../Ayuda/models/persona';

@Injectable({
  providedIn: 'root'
})
export class PersonaService {

  constructor() { }

  get(): Persona[] {
        return JSON.parse(localStorage.getItem('informacion'));
      }
    ValidarPersona (persona:Persona,personas:Persona[]):boolean{
      for(var i in personas){
        if(persona.identificacion==personas[i].identificacion){return false;}        

      }
      return true;
    }
    post(persona: Persona):boolean {
          let personas: Persona[] = [];
          if (this.get() != null) {
            personas = this.get();
          }
      if(this.ValidarPersona(persona,personas)){
          personas.push(persona);
          localStorage.setItem('informacion', JSON.stringify(personas));
      return true;
      }
      return false;
          
    }      
}
