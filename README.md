# TrackMyBudget API - http://54.151.251.222/swagger

**TrackMyBudget** is a simple .NET Core Web API for managing budget entries. It supports basic CRUD operations to create, view, update, and delete budget records.

## Features
- **Create**: Add a new budget entry (category, amount, start date, end date).
- **View All**: Retrieve a list of all budgets.
- **View by ID**: Get details of a specific budget by ID.
- **Update**: Modify an existing budget entry.
- **Delete**: Remove a budget entry by its ID.

## Endpoints

1. **GET /api/budgets** - Retrieve all budgets.
2. **GET /api/budgets/{id}** - Get a budget by ID.
3. **POST /api/budgets** - Create a new budget.
4. **PUT /api/budgets/{id}** - Update a budget.
5. **DELETE /api/budgets/{id}** - Delete a budget.

## Swagger API Documentation

You can explore and test the API using Swagger:

- **Swagger UI**: Navigate to `https://localhost:<port>/swagger` to access the API documentation and test the endpoints interactively.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/TrackMyBudget.git
   cd TrackMyBudget

2. Build and run the application:
   ```bash
    dotnet run
   ```

----

## Deploy to AWS Using AWS EC2

* EC2 (Elastic Compute Cloud) provides **virtual machines (called instances)** that you can configure to run applications, websites, databases, or any other software.
* EC2 provides a variety of instance types that are optimized for different workloads :
*    - General Purpose: Balanced CPU, memory, and network resources (e.g., t2.micro).
     - Compute Optimized: High-performance processors (e.g., c5.large).
     - Memory Optimized: For memory-intensive applications (e.g., r5.large).
     - Storage Optimized: High input/output operations for storage (e.g., i3.large).
* You can choose different operating systems for your EC2 instance, such as Amazon Linux, Ubuntu, Windows, and others.
* EC2 instances are secured using Security Groups, which act like virtual firewalls. You can define which ports (e.g., SSH on port 22, HTTP on port 80) are open and which IP addresses are allowed to access the instance.
* **Elastic IP**: You can assign a static IP address (called an Elastic IP) to your instance, which remains the same even if you stop and restart the instance.
* Elasticity: EC2 allows you to increase or decrease capacity depending on your workload. You can scale up during busy periods and scale down when demand is low.
  
> 1. Log in to AWS Management Console.
> 2. Go to the EC2 Dashboard.
> 3. Launch a New Instance. Click on "Launch Instance" and choose an <ins>Amazon Linux</ins> AMI (For .NET Core/6 applications, we can use either Amazon Linux 2 or Windows Server).
> 4. Name your instance: Enter a descriptive name : <ins>Track-My-Budget-Instance</ins>.
> 5. Select <ins>Amazon Linux 2 AMI (HVM), SSD Volume Type – 64-bit (x86)</ins>. This is a lightweight, free-tier eligible option.
> 6. Instance Type : <ins>t2.micro or t3.micro (1 vCPU, 1 GB RAM).</ins>
> 7. Key Pair (Login) : You need a key pair to SSH into the instance (for Amazon Linux) or RDP (for Windows).
>    - 7.1. Create a new key pair.
>    - 7.2. Choose RSA for the key pair type.
>    - 7.3. Download the key pair (it will be a .pem file).
> 8. Network Settings :
>    - 8.1. Auto-assign Public IP : Ensure this is enabled so that your instance gets a public IP address. This allows you to access the instance over the internet.
>    - 8.2. VPC : Choose the default VPC unless you’ve set up custom networking.
>    - 8.3. Security Group : Create a new security group with SSH (port 22), HTTP (port 80) and HTTPS (port 443).
> 9. Configure Storage : The default storage size (8 GB) is usually sufficient for running a .NET console application.

-----

#### Connect to Your EC2 Instance

1. Connect via SSH: Open a terminal and run the following command ```ssh -i /path/to/your-key.pem ec2-user@<your-ec2-public-ip>```
```
ssh -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" ec2-user@18.141.139.166
```
   - SSH encrypts the data transferred between your local machine and the remote server, ensuring that sensitive information, like passwords or commands, is protected.
   - You use a command like ```ssh``` followed by the IP address or DNS name of the EC2 instance, and authenticate yourself using a private key (for key-based authentication) or a password.
   - AWS generally provides an SSH key pair (.pem file) when you first launch the EC2 instance. You use this key to securely connect.
      - ```-i "your-key.pem"```: Specifies the private key file used to authenticate.
      - ```ec2-user```: The username for the EC2 instance (this could be different depending on the Linux distribution).
      - ```your-instance-public-ip```: The public IP address of your EC2 instance.
   - SSH typically operates over port <ins>22</ins>, so this port needs to be open in the Security Group of your EC2 instance.
  
2. Once connected, install the .NET Core Runtime and ASP.NET Core runtime using the following commands)
```
sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
sudo yum install dotnet-sdk-8.0 -y
```

3. Deploy .NET Core Application. Publish Application on your local machine: Run this from the clone directory in a **new CMD**. ```C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudget>```.
```
dotnet publish -c Release -o ./publish
```
   - Since you're likely only using HTTP (port 5000) without an SSL certificate configured, you can safely disable HTTPS redirection in your application. So remember to comment out ```// app.UseHttpsRedirection();``` before this publish. 

4. Transfer the Published Files to the EC2 Instance: In the same CMD, run the ```scp -i /path/to/your-key.pem -r ./publish ec2-user@<your-ec2-public-ip>:/home/ec2-user/TrackMyBudget``` :
   Transfer your app’s published files from your local machine to the EC2 instance.
```
scp -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" -r ./publish/* ec2-user@54.151.251.222:/home/ec2-user/TrackMyBudget
```

5. Navigate to the Directory on the EC2 instance: Use the EC2 Instance Terminal.
```
cd /home/ec2-user/TrackMyBudget
```

6. Run the Application: Start your application using dotnet. If dotnet not installed, install first. Remember to navigate to ```cd /home/ec2-user``` when need to install.
```
sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm

sudo yum install dotnet-runtime-8.0

sudo yum install dotnet-sdk-8.0
```

7. Navigate to the Directory on the EC2 instance again and run the application :
```
cd /home/ec2-user/TrackMyBudget
dotnet /home/ec2-user/TrackMyBudget/TrackMyBudget.dll --urls "http://*:5000"
```

8. Step 7: Set Up Nginx as a Reverse Proxy. To make your application publicly accessible on port 80, we'll use Nginx as a reverse proxy.
   - 8.1. Install Nginx:
   -    ```
         sudo yum install nginx -y
        ```
   - 8.2. Start and enable Nginx:
   -    ```
         sudo systemctl start nginx
         sudo systemctl enable nginx
        ```
   - 8.3. Configure Nginx: Open the Nginx configuration file:
   -    ```
         sudo nano /etc/nginx/nginx.conf
        ```
   - 8.4. Find the ```http``` block and replace the existing server block with the following configuration: Save and exit the file (CTRL+X, then Y, then Enter).
   -    ```
         server {
          listen 80;
          server_name 54.151.251.222; // This is the Public IP

          location / {
              proxy_pass http://localhost:5000;
              proxy_http_version 1.1;
              proxy_set_header Upgrade $http_upgrade;
              proxy_set_header Connection keep-alive;
              proxy_set_header Host $host;
              proxy_cache_bypass $http_upgrade;
             }
         }
        ```
        
   - 8.5. Restart Nginx to apply the configuration:
   -    ```
           sudo systemctl restart nginx
        ```

9. Adjust EC2 Security Group. In the AWS Management Console, go to EC2 > Instances > Select your instance > Security Groups. Edit the Inbound Rules to ensure HTTP (port 80) is open to the public.
   - Type: HTTP
   - Port Range: 80
   - Source: 0.0.0.0/0 (or anywhere you want to allow access)

10. Access Your API Publicly
    - ```
         http://54.151.251.222/swagger
      ```
