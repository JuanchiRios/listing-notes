Ejercicio ListingNotes
=======

### Introducción
Esta es una API que permite hacer búsquedas de publicaciones en MercadoLibre dentro de la categoría Computación.
Expone los siguientes endpoints:

`GET /items/search?query=`
Sirve para hacer búsquedas en MercadoLibre a partir de una palabra o frase pasada por el parametro **query** por querystring.
Si el parámetro query no se envía, trae las primeras 50 publicaciones de la categoría Computación.

`GET /items/:id`
Sirve para traer una publicación en particular a partir de su id

### Objetivo
Que el usuario de la API pueda guardar notas sobre las publicaciones utilizando [MongoDB](https://docs.mongodb.org/ecosystem/drivers/csharp/) como base da datos
Crear los siguientes endpoints:

1) `PUT /items/:id` enviando las notas en el body de la request dentro de un json.
Ejemplo de un body:
```
{ notes: "esta publicación me interesa" }
```

2) Modificar los endpoints `GET /items/search?query=` y `GET /items/:id` para que, al devolver las publicaciones correspondientes, éstas incluyan las notas del usuario (en los caso que exista alguna).

3) Desarrollar el endpoint `GET /items/withnotes` que devuelva solamente las publicaciones con notas

4) Mejorar el endpoint creado en el punto 3) para que si se envía el parametro query por querystring realice una búsqueda dentro de las notas y devuelva las publicaciones encontradas.

En todos los puntos desarrollar los test unitarios que crea necesarios.
