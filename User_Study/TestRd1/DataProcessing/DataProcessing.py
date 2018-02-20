%matplotlib notebook	
import csv
from __future__ import print_function
import ipywidgets as widgets
from IPython.display import display
import matplotlib.pyplot as plt 
plt.style.use('ggplot')


# create Sentence class
class Sentence:
	# class variable shared by all instances
	def __init__(self, subject, task, sentenceNo, testing):
		# instance variable unique to each instance
		self.subject = subject
		self.task = task			# 0: transcribe; 1: free conversation
		self.sentenceNo = sentenceNo
		self.testing = testing
		self.P = ""
		self.T = ""
		self.IS = ""
		self.wpm = 0.0
		self.totErrRate = 0.0
		self.keyboard = 0	# 0: win10 Eye Control; 1: tobii Win Control

def readSentences(argFilename, argSentences):
	with open('RawData/'+argFilename+'.csv') as csvDataFile:
		csvReader = csv.reader(csvDataFile)
		next(csvReader, None)
		next(csvReader, None)
		for row in csvReader:
			# write on keyboard clasification
			if 'Transcribe' in argFilename:
				argSentences.append(Sentence(int(argFilename[0]), 0, int(row[0]), int(row[1])))
			elif 'FreeConv' in argFilename:
				argSentences.append(Sentence(int(argFilename[0]), 1, int(row[0]), int(row[1])))
			# handle exceptions
			# if test subject didn't enter anything and skipped this sentence
			if int(row[2]) != 0:
				sentences[-1].wpm = int(row[4])+int(row[5])*0.1**(len(row[5]))
				sentences[-1].totErrRate = int(row[-14])*0.1**(len(row[-14]))
			else:
				sentences[-1].wpm = 0
				sentences[-1].totErrRate = 1
			if 'winEyeControl' in argFilename:
				sentences[-1].keyboard = 0
			elif 'tobiiWinControl' in argFilename:
				sentences[-1].keyboard = 1

def UiControls():
	contrSubject = widgets.Select(
		options=['All', '#1', '#2', '#3'],
		value='All',
		description='Test Subject:',
		disabled=False
		)
	contrTask = widgets.Select(
		options=['All', 'Transcribe', 'Free Conversation'],
		value='All',
		description='Task:',
		disabled=False
		)
	contrTypePerf = widgets.ToggleButtons(
		options=['Speed (wpm)', 'Accuracy (totErrRate)'],
		value='Speed (wpm)',
		description='Type performance',
		disabled=False,
		button_style=''
		)
	display(widgets.HBox([contrTypePerf, contrTask, contrSubject]))

	contrTypePerf.observe(onChange_typePerf)
	contrTask.observe(onChange_task)
	contrSubject.observe(onChange_subject)

def onChange_typePerf(b):
	if b['type'] == 'change' and b['name'] == 'value':
		print(contrTypePerf.value)

def onChange_task(change):
	if change['type'] == 'change' and change['name'] == 'value':
		print('task')

def onChange_subject(change):
	if change['type'] == 'change' and change['name'] == 'value':
		print('subject')

if __name__ == "__main__":
	# read data from .csv
	sentences = []
	readSentences('1Greta_s1Transcribe_winEyeControl', sentences)
	readSentences('1Greta_s2Transcibe_tobiiWinControl', sentences)
	readSentences('2Carlota_s1Transcribe_winEyeControl', sentences)
	readSentences('2Carlota_s2FreeConv_winEyeControl', sentences)
	readSentences('2Carlota_s3Transcribe_tobiiWinControl', sentences)
	readSentences('2Carlota_s4FreeConv_tobiiWinControl', sentences)
	readSentences('3Barbara_s1Transcribe_tobiiWinControl', sentences)
	readSentences('3Barbara_s2FreeConv_tobiiWinControl', sentences)
	readSentences('3Barbara_s3Transcribe_winEyeControl', sentences)
	readSentences('3Barbara_s4FreeConv_winEyeControl', sentences)

	UiControls()

	# calculate wpm, total error-rate on sentence, task, subject, keyboard respectively
	# X: keyboard | Y: typing performance | other variables: subject, task


	# plots
	# using ipywidgets to make interactive plots