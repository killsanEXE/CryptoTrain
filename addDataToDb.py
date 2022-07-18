import json
import sqlite3

with open("bitcoinData.json") as f:
    print("Loading data...")
    data = json.load(f)
    print("Loaded data")
    for i in data:
        i["Price"] = i["Price"][:-1]
        i["Price"] = i["Price"].replace(".", "")
        i["Price"] = i["Price"].replace(",", "")
    print("Converted data to float")

    with sqlite3.connect("API/API.Application/cryptotrain.db") as conn:
        print("Connected to database")
        c = conn.cursor()
        query = c.execute("SELECT * from BTCs")
        query = query.fetchall()
        if(len(query) <= 0):
            print("Starting seed process...")
            id = 0
            for i in data:
                id += 1
                c.execute("""
                    INSERT INTO BTCs(Id, Date, Price) values(?, ?, ?)
                """, (id, i["Date"], i["Price"],))
            print("Done")
        else:
            print("BTC table already has data inside")