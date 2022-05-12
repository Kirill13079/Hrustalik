# REST API
The REST API to the app is described below.

## Get Records

### Request

`GET api/record/news`

    curl -X GET http://hrustalik.by/api/record/news 
    
### Response

```json
{
    "id": 8,
    "name": "Врач рассказала, почему после еды может болеть голова",
    "publication": "Мира Маркина",
    "description": "Боль в голове после еды может быть связана с сахарным диабетом и другими нарушениями работы поджелудочной железы, отметила эксперт.",
    "image": "6bce5fe7-ee03-4fdf-9419-3e788f77668e_96741569f77d4eb5116e6e8b0dcc80f1.jpg",
    "isNews": true,
    "isHot": true,
    "isArticle": false,
    "dateAdded": "2022-05-10T20:07:21.3041093+03:00",
    "category": {
        "id": 6,
        "name": "Общая медицина"
    }
}
```

## Login

### Request

`POST api/user/login`

    curl -X POST http://hrustalik.by/api/user/login -H "Content-Type: application/json" --data-binary @- <<DATA
    {
        "email" : "email@gmail.com",
        "password" : "password"
    }
    DATA
    
### Response

```json
{
    "customer": {
        "id": 1,
        "firstName": null,
        "lastName": null,
        "email": "email@gmail.com",
        "bookmarks": null
    },
    "token": "your token"
}
```

## Registration

### Request

`POST api/user/register`

    curl -X POST http://hrustalik.by/api/user/register -H "Content-Type: application/json" --data-binary @- <<DATA
    {
        "email" : "email@gmail.com",
        "password" : "password"
    }
    DATA
    
### Response

```json
{
    "customer": {
        "id": 1,
        "firstName": null,
        "lastName": null,
        "email": "email@gmail.com",
        "bookmarks": null
    }
}
```

