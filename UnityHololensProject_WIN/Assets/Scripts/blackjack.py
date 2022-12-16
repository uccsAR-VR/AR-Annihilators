import numpy as np
import sys

class blackjack:

    def __init__(self):
        self.hands = 1
        #Card ordering:    [x,x,2,3,4,5,6,7,8,9,10,J,Q,K,A]
        self.deckspawner = [0,0,4,4,4,4,4,4,4,4,4 ,4,4,4,4]
        self.userhand = []
        self.usersecondhand = []
        self.dealerhand = []
        self.payratio = 2
    
    def shuffle(self):
        self.deck = self.deckspawner*self.hands

    def newhand(self,shuffleafterhand = False):
        self.userhand = []
        self.dealerhand = []
        self.usersecondhand = []
        if shuffleafterhand:
            self.shuffle()


    def cardplayed(self,card):
        #card is a tuple of (number,player)
        #example valid values
        #(14,0)
        #(3,2)
        #first value is integer representation of card number
        # 2 3 4 5 6 7 8 9 10  J  Q  K  A
        # 2 3 4 5 6 7 8 9 10 11 12 13 14
        #second value is 0=user;1=dealer;2=other;3=user2ndhand

        self.deck[card[0]] -= 1
        if card[1] == 0:
            self.userhand.append(card[0])
        elif card[1] == 1:
            self.dealerhand.append(card[0])
        elif card[1] == 3:
            self.usersecondhand.append(card[0])

    def split(self):
        self.userhand = [self.userhand[0]]
        self.usersecondhand = [self.userhand[0]]
    
    def tree(self,start,deck0):
        deck = deck0.copy()
        tree1 = np.add([1,2,3,4,5,6,7,8,9,10,11], start)
        #[A(1),2,3,4,5,6,7,8,9,10,J,Q,K,A(11)]
        probs = np.zeros(6)
        for i,num in enumerate(tree1):
            if i ==0 or i == 10:
                count = deck[14]
            elif i<9:
                count = deck[i]
            else:
                count = np.sum(deck[10:14])
            if count>0:
                if 17<=num<=21:
                    probs[num-17] += count/54/self.hands
                    tree1[i] = 0
                elif num>21:
                    probs[-1] += count/54/self.hands
                    tree1[i] = 0
                elif num>0:
                    smalldeck = deck.copy()
                    smalldeck[i] -= 1
                    smallprobs = self.tree(num,smalldeck)
                    probs += smallprobs*count/54/self.hands
            else:
                tree1[i] = 0
        return probs



    def getmoves(self):
        #move probabilities stored in stand, hit, split
        #edit: not dealing with split for now
        self.dealernums = np.zeros(6)#probabilities from 17 to 22,22 being anything over 21
        start = self.dealerhand[0]
        self.dealernums = self.tree(start,self.deck)
        self.splitp = None

        if len(self.usersecondhand) == 0:
            if len(self.userhand)==2 and self.userhand[0] == self.userhand[1]:
                #determine whether to split or not
                w1 = np.max(self.getprobs(self.userhand[0]))

                w2 = np.max(self.getprobs(self.userhand))

                w1 = (w1*self.payratio-1)*2
                #w1= (w1-(1-1/self.payratio))*2
                w2 = (w2*self.payratio-1)
                #w2 = w2-(1-1/self.paratio)

                if w1>w2:
                    self.splitp = True
                    #Could add factor of success
                    self.splitf = w1/w2
                else:
                    self.splitp = False

            self.standp,self.hitp = self.getprobs(self.userhand)
        else:
            self.standp,self.hitp = self.getprobs(self.userhand)
            self.standp2,self.hitp2 = self.getprobs(self.usersecondhand)








    def getprobs(self,hand):

        start = np.sum(hand)

        if start < 18:
            standp = self.dealernums[-1]
        else:
            standp = np.sum(self.dealernums[:start-17])

        self.usernums = self.tree(start,self.deck)
        hitp  = 1 - self.dealernums[-2]#21
        hitp -= self.dealernums[-3]*(1-self.usernums[-2])#20
        hitp -= self.dealernums[-4]*(1-np.sum(self.usernums[-3:-1]))#19
        hitp -= self.dealernums[-5]*(1-np.sum(self.usernums[-4:-1]))#18
        hitp -= self.dealernums[-6]*(1-np.sum(self.usernums[-5:-1]))#17      

        return standp,hitp      

    def printprobs(self):
        print(f"Stand probability = {int(self.standp*10000)/100}%")
        str1 = f"{int(self.standp*10000)/100}\n"
        print(f"Hit probability = {int(self.hitp*10000)/100}%")
        str1 += f"{int(self.hitp*10000)/100}\n"
        print(f"Split? = {self.splitp}")
        str1 += f"{self.splitp}"
        f = open("probs.txt", "a")
        f.write(str1)
        f.close()

    def printsum(self, sum1Player, sum2Player):
        '''if (sum1Player==10 or sum1Player==11 or sum1Player==12 or sum1Player==12 or sum1Player==13) :
            sum1 = 10
        else:
            sum1 = sum1Player

        if (sum2Player==10 or sum2Player==11 or sum2Player==12 or sum2Player==12 or sum2Player==13) :
            sum2 = 10
        else:
            sum2 = sum2Player'''
        
        '''str1 = f"{sum1}\n"
        str1 += f"{sum2}\n"'''
        str1 = f"{sum1Player}\n"
        str1 += f"{sum2Player}\n"
        f = open("probs.txt", "w")
        f.write(str1)




    
if __name__ == "__main__":

    str1 = ""
    tempNum = 0
    addPlayerCards = False
    addDealerCards = False
    addDealerCards2 = False
    sumPlayer = 0
    sumDealer = 0
    inlineArgsLength = len(sys.argv)
    print("Total arguments passed: ", inlineArgsLength)
    print("\nName of Python script:", sys.argv[0])
    print("\nArguments passed:", end = " ")
    for i in range(1, inlineArgsLength):
        print(sys.argv[i], end = " ")
        str1 += (sys.argv[i] + " ")
    f = open("test_inline_args.txt", "w")
    f.write(str1)

    bj = blackjack()

    bj.newhand(shuffleafterhand=True)
    '''bj.cardplayed((9,1)) #Dealer gets 9
    bj.cardplayed((14,0)) #Dealer gets Q
    bj.cardplayed((6,1)) #Dealer gets 9
    bj.cardplayed((14,0)) #Dealer gets 9
    bj.printsum(9,12) # first arg is player, second arg. is dealer.'''

    print("here")
    for j in range(1, inlineArgsLength):
        '''print(sys.argv[j], end = " ")'''
        
        if(sys.argv[j] == "d"):
            addPlayerCards = False

        if(addPlayerCards):
            tempNum = int((sys.argv[j]))
            print("card played player: " + str(tempNum) + "\n")
            bj.cardplayed((tempNum, 0)) #User gets a card
            sumPlayer += tempNum

        if(addDealerCards):
            tempNum = int((sys.argv[j]))
            print("card played dealer: " + str(tempNum) + "\n")
            bj.cardplayed((tempNum, 1)) #Dealer gets a card
            sumDealer += tempNum

        '''if(addDealerCards):
            addDealerCards2 = True
            addDealerCards = False'''

        if(sys.argv[j] == "p"):
            addPlayerCards = True
            
        if(sys.argv[j] == "d"):
            addDealerCards = True
            addPlayerCards = False

    '''bj.cardplayed((9,1)) #Dealer gets 9
    bj.cardplayed((5,0)) #User gets 5
    bj.cardplayed((7,1)) #Dealer gets 7
    bj.cardplayed((5,0)) #User gets 5
    bj.cardplayed((3,0)) #User gets 3

    bj.printsum(13,16) # first arg is player, second arg. is dealer.'''
    bj.printsum(sumPlayer,sumDealer)
    bj.getmoves()
    bj.printprobs()
