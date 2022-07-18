import json
import sqlite3

with open("bitcoinData.json") as f:
    data = json.load(f)
    with sqlite3.connect("API/API.Application/cryptotrain.db") as conn:
        c = conn.cursor()
        query = c.execute("SELECT * from BTCs")
        query = query.fetchall()
        if(len(query) <= 0):
            id = 0
            for i in data:
                id += 1
                c.execute("""
                    INSERT INTO BTCs(Id, Date, Price) values(?, ?, ?)
                """, (id, i["Date"], i["Price"],))
        