# API Inspektor .NetCore
API desarrollada para la consulta y verificación de información relacionada con listas restrictivas, vinculantes, inhibitorias o condicionantes, informativas y de PEPs para la simplificación de procesos de conocimiento de terceros y debida diligencia. El API está soportado por Inspektor®. Cuenta con el método principal, el cual realiza la consulta de todas las listas y un método destinado para la creación y asignación de tokens JWT para nuevos usuarios.

<!-- ABOUT THE PROJECT -->
## Sobre el aplicativo

El API ha sido desarrolla bajo la plataforma `.NET Core`, especificamente la versión **6**. cuenta con **SWAGGER** para realizar pruebas directamente desde el navegador y también hace parte de la documentación del aplicativo.

### Aspectos a tener en cuenta:
* Si bien se puede usar directamente SWAGGER para hacer uso del servicio, este también puede ser consumido a través de la aplicación <a href="https://www.postman.com/downloads/?utm_source=postman-home">POSTMAN</a>. El repositorio cuenta con un archivo `Json` llamado `API Inspektor.postman_collection` que contiene una colección creada para las pruebas en el **API**.
* Las solicitudes configuradas en el servicio son del tipo **POST**, por lo tanto, la estrcutura de las consultas es el siguiente: 
  - **Consulta principal:**
    ```json
    {    
      "nombre": "string",  
      "identificacion": "string",
      "cantidadPalabras": "string",
      "tienePrioridad_4": true/false
    }
    ``` 
   - **Consulta para generación de token JWT:**
      ```json
      {
          "Usuario":"string",
          "Contrasena":"string",
          "IdUsuarioToken": 0
      }
      ``` 
* El método destinado a la creación y asignación de los tokens **JWT** se encuentra oculto para el su uso a través de **SWAGGER**. Sin embargo, en la colección `Json` nombrada anteriormente hay una consulta ya configurada para ese método.
* **TODOS** Los tokens generados por el **API** tienen un tiempo de validez finito configurado a 1 año de vigencia por cada token creado y están asociados a un usuario registrado en **Inspektor** previa validación de su estado de activación en la Base de datos.
* Todos los parámetros de configuración para la *generación/validación* de los tokens se encuentran en `appsettings.json`; el tiempo de vigencia del token está definido en **minutos**.



<p align="right"><a href="#top">🔼</a></p>
