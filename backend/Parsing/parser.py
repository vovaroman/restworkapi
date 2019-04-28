import urllib.request
import urllib.parse
import sol



class Parser:
    def __init__(self):
        self.url = sol.vacancy_link
    def parse(self):
        f = urllib.request.urlopen(self.url)
        file = open("testfile.txt","w") 
        file.write(f.read().decode('utf-8')) 
        file.close()
        #print(f.read().decode('utf-8'))

        pass