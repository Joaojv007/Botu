import pandas as pd
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time

#configuracoes iniciais
service = Service()
options = webdriver.ChromeOptions() 
options.headless = False
driver = webdriver.Chrome(service=service, options=options)
url = 'https://siga.udesc.br/sigaSecurityG5/login.jsf?tipoLogin=PADRAO&motivo=SESSAO_EXPIRADA&evento=logout&uri-retorno=login.jsf&execIframe=&codigoSistemaLogout='

#ir até o siga
driver.get(url)

# Localize o campo de login e insira o texto
login = driver.find_element(By.ID, 'j_username')
login.send_keys('12446363989')  # Insira o login desejado
print('Login inserido')

# Continue com outras ações, por exemplo, inserindo a senha
senha = driver.find_element(By.ID, 'senha')
senha.send_keys('Jv5626$$')  # Insira a senha desejada
print('Senha inserida')

btnEntrar = driver.find_element(By.ID, 'btnLogin')
btnEntrar.click()
print('entrando')
time.sleep(5)

# Encontrar o elemento do link
link_element = WebDriverWait(driver, 10).until(
    EC.presence_of_element_located((By.XPATH, "//div[@class='ds-painelDeLinks' and @title='Notas e faltas']/a"))
)
link_element.click()
print('Aba de notas')
time.sleep(5)

# Encontrar a tabela de notas e faltas
tabela = driver.find_element(By.ID, 'formPrincipal:notasFaltas_data')

# Encontrar todas as linhas da tabela
linhas = tabela.find_elements(By.TAG_NAME, 'tr')

# Inicializar uma lista vazia para armazenar as informações
informacoes = []

# Iterar sobre as linhas da tabela, começando da segunda linha (pois a primeira é o cabeçalho)
for linha in linhas[1:]:
    # Encontrar os elementos de cada coluna na linha atual
    colunas = linha.find_elements(By.TAG_NAME, 'td')
    
    # Extrair informações específicas de cada coluna
    disciplina = colunas[0].text
    nota = colunas[1].text if colunas[1].text else None
    faltas = colunas[2].text
    
    # Armazenar as informações em um dicionário e adicionar à lista de informações
    info_disciplina = {
        'Disciplina': disciplina,
        'Nota': nota,
        'Faltas': faltas
    }
    informacoes.append(info_disciplina)

# Exibir as informações coletadas
for info in informacoes:
    print(info)