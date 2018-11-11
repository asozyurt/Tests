import pyodbc
from yandex import Yandex
from account import Account

#Create Context


account = Account()
account.generate_account()

yandex = Yandex(account,CAPTCHAAPI,PROXY_USER,PROXY_USER_PASSWORD)
yandex.create_account()

con = pyodbc.connect(driver="{SQL Server}",server=DBSERVER,database=DBDATABASEINSTANCE,uid=DBUSERNAME,pwd=DBPASSWORD)
db_cmd = "INSERT INTO [dbo].[ImaginaryUsers] VALUES ('EmailCreated','"+yandex.account.mail+"','"+account.firstName+"','"+account.lastName+"','"+yandex.account.mail+"@yandex.com',null,'"+account.gender+"','"+account.birthYear+"-"+account.birthMonth+"-"+account.birthDay+"',GETDATE(),'"+workerUser+"',GETDATE(),'"+CURRENT_USER+"','"+account.password+"',NULL,NULL)"
print (db_cmd)
con.execute(db_cmd)
con.commit()
