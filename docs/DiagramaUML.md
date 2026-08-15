# Diagrama UML - Clínica Veterinaria ZeusPet

En una clínica veterinaria el **paciente es la mascota** (la que se atiende) y el
**Cliente es el dueño**. Un dueño puede tener una o varias mascotas.

```mermaid
classDiagram
    direction LR

    class Cliente {
        -string _nombre
        -int _edad
        -string _telefono
        -string _email
        -string _direccion
        +string Id
        +string Nombre
        +int Edad
        +string Telefono
        +string Email
        +string Direccion
        +List~Mascota~ Mascotas
        +MostrarInformacion() void
        +MostrarMascotas() void
        +Registrar() string
    }

    class Mascota {
        -string _raza
        +string Id
        +string Nombre
        +string Especie
        +int Edad
        +string Raza
        +string ClienteId
        +MostrarInformacion() void
        +EmitirSonido() string
        +Registrar() string
    }

    class Animal {
        -string _nombre
        -int _edad
        -string _especie
        +string Nombre
        +int Edad
        +string Especie
        +EmitirSonido() string
        +MostrarInformacion() void
    }

    class IRegistrable {
        <<interface>>
        +Registrar() string
    }

    class ServicioVeterinario {
        <<abstract>>
        +Atender() string
    }

    class ConsultaGeneral {
        +Atender() string
    }

    class Vacunacion {
        +Atender() string
    }

    Animal <|-- Mascota
    IRegistrable <|.. Cliente
    IRegistrable <|.. Mascota
    ServicioVeterinario <|-- ConsultaGeneral
    ServicioVeterinario <|-- Vacunacion
    Cliente "1" --> "0..*" Mascota : posee
```

## Relaciones

- **Asociación (dueño → mascota):** un `Cliente` posee de 0 a muchas `Mascota`
  (lista `List<Mascota>`). La mascota guarda la referencia al dueño mediante `ClienteId`.
- **Herencia:** `Mascota` hereda de `Animal` (nombre, edad, especie) y agrega
  `Raza`, `Id` y `ClienteId`.
- **Abstracción:** `ServicioVeterinario` es abstracta; `ConsultaGeneral` y
  `Vacunacion` sobrescriben `Atender()`.
- **Interfaz:** `IRegistrable` con `Registrar()` la implementan `Cliente` y `Mascota`.
