%matplotlib notebook	
import csv
from __future__ import print_function
import ipywidgets as widgets
from IPython.display import display, clear_output
import matplotlib.pyplot as plt 
plt.style.use('ggplot')
import numpy as np


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
				argSentences.append(Sentence(int(argFilename[0])-1, 0, int(row[0]), int(row[1])))
			elif 'FreeConv' in argFilename:
				argSentences.append(Sentence(int(argFilename[0])-1, 1, int(row[0]), int(row[1])))
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

def onChange_plotType(b):
	if b['type'] == 'change' and b['name'] == 'value':
		plots()
		print(contrTypePerf.value)

def onChange_task(change):
	if change['type'] == 'change' and change['name'] == 'value':
		plots()

def onChange_subject(change):
	if change['type'] == 'change' and change['name'] == 'value':
		plots()

# def delOutliers(argSentenceSet):
# 	print('did not filter')

# read task and subject selection from UI controls, prepare for plotting
def plots():
	print('plots')
	# read subject selection from UI control
	if contrTask.value == 'Transcribe':
		argTask = 0
	elif contrTask.value == 'Free Conversation':
		argTask = 1
	if contrSubject.value != '#All':
		argSubject = int(contrSubject.value[1])-1
		sentenceKeybdA = [item for item in sentences if item.subject == argSubject and item.task == argTask and item.keyboard == 0 and item.testing == 1]
		sentenceKeybdB = [item for item in sentences if item.subject == argSubject and item.task == argTask and item.keyboard == 1 and item.testing == 1]			
	else:
		argSubject = '#All'
		sentencesKeybdA = [item for item in sentences if item.task == argTask and item.keyboard == 0 and item.testing == 1]
		sentencesKeybdB = [item for item in sentences if item.task == argTask and item.keyboard == 1 and item.testing == 1]

	if contrPlotType.value == 'Speed over sentences':
		if argSubject != '#All':
			plotSentences(sentenceKeybdA, sentenceKeybdB, 1, 1)	
		else:
			plotSentences(sentencesKeybdA, sentencesKeybdB, 3, 3)		


# plot TypePerf of 2 keyboards against sentences, with variable variance visualization,
def plotSentences(argSentenceSet0, argSentenceSet1, argCount0, argCount1):
	# print('plotSentences')
	sentenceKeybdA, sentenceKeybdB = ([] for i in range(2))
	counter = 0
	counterTrim = [[0],[0]]
	subject = argSentenceSet0[0].subject
	# read sentences for a keyboard within a task
	for item in argSentenceSet0:
		if argSentenceSet0[counter].subject != subject:
			counterTrim[0].append(counter)
			subject = argSentenceSet0[counter].subject
		counter += 1
	counterTrim[0].append(len(argSentenceSet0))
	for item in counterTrim[0]:
		if item != 0:
			sentenceKeybdA.append(argSentenceSet0[itemTemp:item])
		itemTemp = item
	counter = 0
	for item in argSentenceSet1:
		if argSentenceSet1[counter].subject != subject:
			counterTrim[1].append(counter)
			subject = argSentenceSet1[counter].subject
		counter += 1
	counterTrim[1].append(len(argSentenceSet1))
	for item in counterTrim[1]:
		if item != 0:
			sentenceKeybdB.append(argSentenceSet1[itemTemp:item])
		itemTemp = item

	# handle exceptions when wpm = 0, replace it with gradient
	x_A, y_A, z_A, yMean_A, x_B, y_B, z_B, yMean_B = ([] for i in range(8))
	for itemI in sentenceKeybdA:
		xTemp, yTemp, zTemp = ([] for i in range(3))
		for itemJ in itemI:
			xTemp.append(itemJ.sentenceNo)
			yTemp.append(itemJ.wpm)
			zTemp.append(itemJ.totErrRate)
		x_A.append(xTemp)
		y_A.append(yTemp)
		z_A.append(zTemp)
	for itemI in sentenceKeybdB:
		xTemp, yTemp, zTemp = ([] for i in range(3))
		for itemJ in itemI:
			xTemp.append(itemJ.sentenceNo)
			yTemp.append(itemJ.wpm)
			zTemp.append(itemJ.totErrRate)
		x_B.append(xTemp)
		y_B.append(yTemp)
		z_B.append(zTemp)

	# plot average horizontal line
	length = [0, 0]
	for j in range(2):
		for i in range(len(counterTrim[j])-2):
			if len(y_A[i]) > length[j]:
				length[j] = len(y_A[i])

	for i in range(length[0]):
		yMeanBuffer = []
		for j in range(len(counterTrim[0])-1):
			try:
				yMeanBuffer.append(y_A[j][i])
			except IndexError:
				pass
		yMeanValue = np.mean(yMeanBuffer)
		yMean_A.append(yMeanValue)
	for i in range(length[1]):
		yMeanBuffer = []
		for j in range(len(counterTrim[1])-1):
			try:
				yMeanBuffer.append(y_B[j][i])
			except IndexError:
				pass
		yMeanValue = np.mean(yMeanBuffer)
		yMean_B.append(yMeanValue)

	fig, ax = plt.subplots()
	for i in range(len(sentenceKeybdA)):
		plt.scatter(x_A[i], y_A[i], c=z_A[i])
		plt.scatter(x_B[i], y_B[i], c=z_B[i])
		if argCount0 == 1: 
			plt.plot(x_A[i], y_A[i], 'o-', label='win10 EyeControl', alpha=0.2, color='orange')
			yMeanValue_A = np.mean(y_A[i])
			plt.plot([min(x_A[i]), max(x_A[i])], [yMeanValue_A, yMeanValue_A], '--', label='win10 EyeControl mean', color='orange')
		elif i == 0:
			plt.scatter(x_A[i], y_A[i], color='orange', alpha=0.3, label='win10 EyeControl')
		else:
			plt.scatter(x_A[i], y_A[i], color='orange', alpha=0.3)
		if argCount1 == 1:
			plt.plot(x_B[i], y_B[i], 'o-', label='tobii WinControl', alpha=0.2, color='blue')
			yMeanValue_B = np.mean(y_B[i])
			plt.plot([min(x_B[i]), max(x_B[i])], [yMeanValue_B, yMeanValue_B], '--', label='tobii WinControl mean', color='blue')	
		elif i ==0:
			plt.scatter(x_B[i], y_B[i], color='blue', alpha=0.3, label='tobii WinControl')
		else:
			plt.scatter(x_B[i], y_B[i], color='blue', alpha=0.3)
	if argCount0 != 1:
		plt.plot(range(4, len(yMean_A)+4), yMean_A, '--', label='win10 EyeControl mean', color='orange')
	if argCount1 != 1:
		plt.plot(range(4, len(yMean_B)+4), yMean_B, '--', label='tobii WinControl mean', color='blue')

	# ax.legend(loc='upper center', bbox_to_anchor=(0.5, -0.05), ncol=4)
	ax.legend(loc='upper left')
	ax.set(title='Subject '+contrSubject.value, xlabel='sentence', ylabel='wpm')
	plt.set_cmap('gray')	# high error rate deepen the scatter color
	plt.show()

def plotWpmBetweenTasks():
	# sentenceTask1=[[sentences for task0],[sentences for task1]]
	sentenceTask0, sentenceTask1 = ([] for i in range(2))
	# 3 subjects, so tempSentenceTask0 = [[]*3]
	tempSentenceTask0Keybd0, tempSentenceTask0Keybd1, tempSentenceTask1Keybd0, tempSentenceTask1Keybd1 = ([[], [], []] for i in range(4))
	for item in sentences:
		if item.task == 0:
			if item.keyboard == 0:
				tempSentenceTask0Keybd0[item.subject].append(item)
			elif item.keyboard == 1:
				tempSentenceTask0Keybd1[item.subject].append(item)
		elif item.task == 1:
			if item.keyboard == 0:
				tempSentenceTask1Keybd0[item.subject].append(item)
			elif item.keyboard == 1:
				tempSentenceTask1Keybd1[item.subject].append(item)
	# indexSubjectTask: row: within task; column: within subject
	indexSubjectTask = [[len(tempSentenceTask0Keybd0[0]), len(tempSentenceTask0Keybd0[1]), len(tempSentenceTask0Keybd0[2])],
						[len(tempSentenceTask0Keybd1[0]), len(tempSentenceTask0Keybd1[1]), len(tempSentenceTask0Keybd1[2])],
						[len(tempSentenceTask1Keybd0[0]), len(tempSentenceTask1Keybd0[1]), len(tempSentenceTask1Keybd0[2])],
						[len(tempSentenceTask1Keybd1[0]), len(tempSentenceTask1Keybd1[1]), len(tempSentenceTask1Keybd1[2])]]
	sentenceTask0Keybd0 = tempSentenceTask0Keybd0[0] + tempSentenceTask0Keybd0[1] + tempSentenceTask0Keybd0[2]
	sentenceTask0Keybd1 = tempSentenceTask0Keybd1[0] + tempSentenceTask0Keybd1[1] + tempSentenceTask0Keybd1[2]
	sentenceTask1Keybd0 = tempSentenceTask1Keybd0[0] + tempSentenceTask1Keybd0[1] + tempSentenceTask1Keybd0[2]
	sentenceTask1Keybd1 = tempSentenceTask1Keybd1[0] + tempSentenceTask1Keybd1[1] + tempSentenceTask1Keybd1[2]

	# wpmMeanTaskN = [[0]*m*n], m=amount of keyboards , n = amount of testing subjects
	wpmMeanTask0, wpmMeanTask1 = ([[0]*(amountSubject+1)]*2 for i in range(2))
	for item in sentenceTask0Keybd0:
		wpmMeanTask0[0][item.subject] += item.wpm
		wpmMeanTask0[0][amountSubject] += item.wpm
	# for index in range(len(indexSubjectTask[0])):
	# 	wpmMeanTask0[0][index] /= indexSubjectTask[0][index]
	# wpmMeanTask0[0][amountSubject] /= sum(indexSubjectTask[0])
	for item in sentenceTask0Keybd1:
		wpmMeanTask0[1][item.subject] += item.wpm
		wpmMeanTask0[1][amountSubject] += item.wpm
	for i in range(2):
		for index in range(len(indexSubjectTask[i])):
			wpmMeanTask0[i][index] /= indexSubjectTask[i][index]
		wpmMeanTask0[i][amountSubject] /= sum(indexSubjectTask[i])	

	for item in sentenceTask1Keybd0:
		wpmMeanTask1[0][item.subject] += item.wpm
		wpmMeanTask1[0][amountSubject] += item.wpm
	for item in sentenceTask1Keybd1:
		wpmMeanTask0[1][item.subject] += item.wpm
		wpmMeanTask0[1][amountSubject] += item.wpm
	# for i in range(2):
	# 	for index in range(len(indexSubjectTask[2+i])):
	# 		wpmMeanTask1[i][index] /= indexSubjectTask[i][index]
	# 	wpmMeanTask1[i][amountSubject] /= sum(indexSubjectTask[i])	

	# for index in range(len(indexSubjectTask[1])):
	# 	try:
	# 		wpmMeanTask1[index] /= indexSubjectTask[1][index]		
	# 	except ZeroDivisionError:
	# 		pass
	# wpmMeanTask1[amountSubject] /= sum(indexSubjectTask[1])

	xTemp1=range(16)
	# xForPlot = [range(16), range(10,18)]
	fig, ax = plt.subplots()
	plt.bar(xTemp1, wpmMeanTask0[0]+wpmMeanTask0[1]+wpmMeanTask1[0]+wpmMeanTask1[1], color="gray")
	plt.show()

	# bug: calculate mean, and two keyboards shouldnt have the same value


if __name__ == "__main__":
	# read data from .csv
	sentences = []
	amountSubject = 3
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

	# UI controls
	contrSubject = widgets.Select(
		options=['#All', '#1', '#2', '#3'],
		value='#3',
		description='Test Subject:',
		disabled=False
		)
	contrTask = widgets.Select(
		options=['Transcribe', 'Free Conversation'],
		value='Transcribe',
		description='Task:',
		disabled=False
		)
	contrPlotType = widgets.ToggleButtons(
		options=['Speed over sentences', 'Speed between tasks', 'Speed vs. accuracy', 'Irrelavant variables influence'],
		# value='Speed over sentences',
		description='Plot Type:',
		disabled=False,
		button_style=''
		)
	display(contrPlotType, widgets.HBox([contrTask, contrSubject]))
	contrPlotType.observe(onChange_plotType)
	contrTask.observe(onChange_task)
	contrSubject.observe(onChange_subject)

	# plots()
	plotWpmBetweenTasks()