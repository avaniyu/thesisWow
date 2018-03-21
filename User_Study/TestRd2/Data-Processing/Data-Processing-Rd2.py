# MVP:
# 3 keyboards, 
# ui control: 
# speed (wpm, adjWpm)and/or accuracy (totErrRate);
# FOA distribution ratio
# plot: sentenceNo, SD

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
	def __init__(self, participant, sentenceNo, testing):
		# instance variables here
		self.participant = participant
		self.sentenceNo = sentenceNo
		self.testing = testing		# 0: practice; 1: test
		self.keyboard = 0			# 0: win EyeControl; 1: tobii WinControl; 2: tobii Dwell-free
		self.wpm = 0.0
		self.adjWpm = 0.0
		self.totErrRate = 0.0
		self.UncErrRate = 0.0
		self.CorErrRate = 0.0

def readSentences(argFilename, argSentences):
	pass
	with open('Raw-Data/Logs/'+argFilename+'.csv') as csvDataFile:
		csvReader = csv.reader(csvDataFile)
		next(csvReader, None)
		next(csvReader, None)
		for row in csvReader:
			argSentences.append(Sentence(int(argFilename[0])-1, row[0], row[1]))
			# exclude exceptions that the test participant didn't enter anything and skipped this sentence			
			if row[3] != 0:			
				sentences[-1].wpm = row[3]
				sentences[-1].adjWpm = row[4]
				sentences[-1].totErrRate = row[11]
				sentences[-1].UncErrRate = row[9]
				sentences[-1].CorErrRate = row[10]				
				sentences[-1].keyboard = ord(argFilename[4])-65

	# read csv file
	# read to sentence class

def UIs():
	contrMetric = widgets.ToggleButtons(
		options=['Speed', 'Accuracy', 'Speed & Accuracy', 'Learning Curve', 'Attention Distribution'],
		description='Metric: ',
		value='Speed',
		disabled=False
		)
	contrPtcp = widgets.Select(
		options=['All', '#1', '#2', '#3', '#4', '#5', '#6'],
		description='Participant: ',
		value='All',
		disabled=False
		)
	display(contrMetric, widgets.HBox([contrPtcp]))
	contrMetric.observe(onChange_metric)
	contrPtcp.observe(onChange_ptcp)

def clearCache():
	clear_output(wait=True)
	display(contrMetric, widgets.HBox([contrPtcp]))
	# clear data storage

def onChange_metric(b):
	if b['type']=='change' and b['name']=='value':
		onChange_ptcp()

def onChange_ptcp(change):
	if change['type']=='change' and change['name']=='value':
		plotsHub()

def plotsHub():
	clearCache()
	print('plotsHub')

def clearCache():
	print('clear cache')

def plotSpeed():
	pass

def plotAccuracy():
	pass

def plotSpeedNAccuracy():
	pass

def plotLearningCurve():
	pass

def plotAttention():
	pass

if __name__=="__main__":
	sentences=[]
	amountPtcp=6
	amountKeyboard=3

	filenames = ['1_kbB_logs', '2_kbB_logs', '3_kbB_logs']
	for item in filenames:
		readSentences(item, sentences)

	UIs()