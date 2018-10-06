%matplotlib notebook	
import csv
from __future__ import print_function
import ipywidgets as widgets
from IPython.display import display, clear_output
import matplotlib.pyplot as plt
plt.style.use('ggplot')
import numpy as np

class Sentence:
	# class variables here
	def __init__(self, sentenceNo, testing):
		# instance variables here
		self.sentenceNo = sentenceNo
		self.testing = testing		# 0: practice; 1: test
		self.wpm = 0.0
		self.adjWpm = 0.0
		self.totErrRate = 0.0
		self.uncErrRate = 0.0
		self.corErrRate = 0.0

def readSentences(argSentences):
	with open('myTyping.csv') as csvDataFile:
		csvReader = csv.reader(csvDataFile)
		next(csvReader, None)
		next(csvReader, None)
		for row in csvReader:
			argSentences.append(Sentence(int(row[0]), int(row[1])))
			# exclude exceptions that the test participant didn't enter anything and skipped this sentence			
			if float(row[3]) != 0:		
				sentences[-1].wpm = float(row[3])
				sentences[-1].adjWpm = float(row[4])
				sentences[-1].totErrRate = float(row[11])
				sentences[-1].uncErrRate = float(row[9])
				sentences[-1].corErrRate = float(row[10])

def getAverageTypingPerformance(argSentences):
	myAdjWpm = []
	myTotErrorRate = []
	for i in range(len(argSentences)):
		myAdjWpm.append(argSentences[i].adjWpm)
	meanAdjWpm = np.mean(myAdjWpm)
	sdAdjWpm = np.std(myAdjWpm)
	myAdjWpm.clear()
	for i in range(len(argSentences)):
		if abs(argSentences[i].adjWpm - meanAdjWpm) <= 2 * sdAdjWpm:
			myAdjWpm.append(argSentences[i].adjWpm)
			myTotErrorRate.append(argSentences[i].totErrRate)
	print(np.mean(myTotErrorRate))
	print(np.std(myTotErrorRate))


if __name__=="__main__":
	sentences=[]
	readSentences(sentences)
	getAverageTypingPerformance(sentences)

