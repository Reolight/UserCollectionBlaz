# UserCollectionBlaz
This application allows you to create and manage your collections.

You should be registered for those actions. As a guest you can view existing collections. As a user you can:
1. Create your collection (YC)
2. Edit YC
3. Delete YC
4. Add various items with images (will be loaded to Cloudinary)
5. Create additional fields to items setting up rules for those on the level of collection (like "author" (string) and "publish date" (number) for books)
6. Add Tags to your item (they are even clickable and redirect you to simple search where you will get all items with tag you clicked)
7. Like items (single like for user [narcissizm (i.e. self-liking) is not prohibited intentionally])
8. Make your collection private (can be seen only by you and admin)
9. Comment collections and items

Simple search is available. It works fine with small data (there are plans to make it fulltext search to solve the problem). Search is able to filter items, collections and messages

There is also simple user panel
You can also find EmailService in the code. It is designed for "forget password" button and requires existing email (the only problem is that there is no check for email existence 
